namespace Uma.DataConnector.Logging
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// Build tracking extension, which handles ILog resolve for Unity
    /// with named LOGGER, name = creating type
    /// http://blog.baltrinic.com/software-development/dotnet/log4net-integration-with-unity-ioc-container
    /// </summary>
    /// <remarks>Unit testing is not possible due to Unity Contained base class</remarks>
    [ExcludeFromCodeCoverage]
    public class BuildTracking : UnityContainerExtension
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
            this.Context.Strategies.AddNew<BuildTrackingStrategy>(UnityBuildStage.TypeMapping);
        }

        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <param name="context">The context.</param>
        public static IBuildTrackingPolicy GetPolicy(IBuilderContext context)
        {
            return context == null ? null : context.Policies.Get<IBuildTrackingPolicy>(context.BuildKey, true);
        }

        /// <summary>
        /// Sets the policy.
        /// </summary>
        /// <param name="context">The context.</param>
        public static IBuildTrackingPolicy SetPolicy(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = new BuildTrackingPolicy();
            if (context != null)
            {
                context.Policies.SetDefault(policy);
            }

            return policy;
        }
    }

    /// <summary>
    /// Build Tracking Strategy class for Unity and NLog
    /// </summary>
    /// <remarks>Excluded from Code coverage due to Unity class wrapper - unable to mock</remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too many wrapper classes for one single purpose better to go in one file.")]
    [ExcludeFromCodeCoverage]
    public class BuildTrackingStrategy : BuilderStrategy
    {
        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Context being null is handled in called methods")]
        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = BuildTracking.GetPolicy(context) ?? BuildTracking.SetPolicy(context);

            policy.BuildKeys.Push(context.BuildKey);
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PostBuildUp method is called when the chain has finished the PreBuildUp
        /// phase and executes in reverse order from the PreBuildUp calls.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PostBuildUp(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = BuildTracking.GetPolicy(context);
            if ((policy != null) && (policy.BuildKeys.Count > 0))
            {
                policy.BuildKeys.Pop();
            }
        }
    }

    /// <summary>
    /// Interface for Build Tracking Policy
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too many wrapper classes for one single purpose better to go in one file.")]
    public interface IBuildTrackingPolicy : IBuilderPolicy
    {
        /// <summary>
        /// Gets the build keys.
        /// </summary>
        Stack<object> BuildKeys { get; }
    }

    /// <summary>
    /// Implementation of Build Tracking policy for Unity and logging
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too many wrapper classes for one single purpose better to go in one file.")]
    [ExcludeFromCodeCoverage]
    public class BuildTrackingPolicy : IBuildTrackingPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTrackingPolicy"/> class.
        /// </summary>
        public BuildTrackingPolicy()
        {
            this.BuildKeys = new Stack<object>();
        }

        /// <summary>
        /// Gets the build keys.
        /// </summary>
        public Stack<object> BuildKeys { get; private set; }
    }
}
