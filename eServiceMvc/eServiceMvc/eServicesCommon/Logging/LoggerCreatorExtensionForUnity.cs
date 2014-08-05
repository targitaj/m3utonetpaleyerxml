namespace Uma.Eservices.Common
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// Build tracking extension, which handles ILog resolve for Unity
    /// with named LOGGER, name = creating type
    /// http://blog.baltrinic.com/software-development/dotnet/log4net-integration-with-unity-ioc-container
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LogCreation : UnityContainerExtension
    {
        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        /// <see cref="T:Microsoft.Practices.Unity.ExtensionContext" /> by adding strategies, policies, etc. to
        /// install it's functions into the container.
        /// </remarks>
        protected override void Initialize()
        {
            Context.Strategies.AddNew<LogCreationStrategy>(UnityBuildStage.PreCreation);
        }
    }

    /// <summary>
    /// Strategy for Unity to create Log onject
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too many wrapper classes for one single purpose better to go in one file.")]
    [ExcludeFromCodeCoverage]
    public class LogCreationStrategy : BuilderStrategy
    {
        /// <summary>
        /// Gets a value indicating whether policy is already set.
        /// </summary>
        public bool IsPolicySet { get; private set; }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context == null)
            {
                return;
            }

            Type typeToBuild = context.BuildKey.Type;
            if (typeof(ILog).Equals(typeToBuild))
            {
                if (context.Policies.Get<IBuildPlanPolicy>(context.BuildKey) != null)
                {
                    return;
                }

                Type typeForLog = LogCreationStrategy.GetLogType(context);
                IBuildPlanPolicy policy = new LogBuildPlanPolicy(typeForLog);
                context.Policies.Set<IBuildPlanPolicy>(policy, context.BuildKey);
                this.IsPolicySet = true;
            }
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PostBuildUp method is called when the chain has finished the PreBuildUp
        /// phase and executes in reverse order from the PreBuildUp calls.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context == null)
            {
                return;
            }

            if (this.IsPolicySet)
            {
                context.Policies.Clear<IBuildPlanPolicy>(context.BuildKey);
                this.IsPolicySet = false;
            }
        }

        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <param name="context">The context.</param>
        private static Type GetLogType(IBuilderContext context)
        {
            Type logType = null;
            IBuildTrackingPolicy buildTrackingPolicy = BuildTracking.GetPolicy(context);
            if ((buildTrackingPolicy != null) && (buildTrackingPolicy.BuildKeys.Count >= 2))
            {
                logType = ((NamedTypeBuildKey)buildTrackingPolicy.BuildKeys.ElementAt(1)).Type;
            }
            else
            {
                StackTrace stackTrace = new StackTrace();

                // first two are in the log creation strategy, can skip over them
                for (int i = 2; i < stackTrace.FrameCount; i++)
                {
                    StackFrame frame = stackTrace.GetFrame(i);
                    logType = frame.GetMethod().DeclaringType;
                    if (!logType.FullName.StartsWith("Microsoft.Practices", true, CultureInfo.InvariantCulture))
                    {
                        break;
                    }
                }
            }
            return logType;
        }
    }

    /// <summary>
    /// Log Build Plan policy
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too many wrapper classes for one single purpose better to go in one file.")]
    [ExcludeFromCodeCoverage]
    public class LogBuildPlanPolicy : IBuildPlanPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogBuildPlanPolicy"/> class.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        public LogBuildPlanPolicy(Type logType)
        {
            this.LogType = logType;
        }

        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>
        /// The type of the log.
        /// </value>
        public Type LogType { get; private set; }

        /// <summary>
        /// Creates an instance of this build plan's type, or fills
        /// in the existing type if passed in.
        /// </summary>
        /// <param name="context">Context used to build up the object.</param>
        public void BuildUp(IBuilderContext context)
        {
            if (context != null && context.Existing == null)
            {
                ILog log = new Log(this.LogType.Name);
                context.Existing = log;
            }
        }
    }
}
