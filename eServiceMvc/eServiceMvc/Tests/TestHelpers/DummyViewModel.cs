namespace Uma.Eservices.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Attributes;

    /// <summary>
    /// Dummy class to mimic some general View Model (which should be simple POCO)
    /// and it contains basically all property types to test against, named
    /// {type}Property
    /// Note: Add more if you need in your tests. Even create subobjects and lists/dictionaries.
    /// Note: There is a method in <see cref="RandomData" /> that generates actual object
    /// of this Dummy ViewModel with random contents for its values
    /// </summary>
    [Validator(typeof(DummyViewModelValidator))]
    public class DummyViewModel
    {
        public bool BoolProperty { get; set; }
        public bool BoolPropertyTrue { get { return true; } }
        public byte ByteProperty { get; set; }
        public short ShortProperty { get; set; }
        public int IntProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public float FloatProperty { get; set; }
        public string StringProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public Guid GuidProperty { get; set; }
        public TestEnum TestEnumProperty { get; set; }
        public TestEnumSmall TestEnumSmallProperty { get; set; }
        public Dictionary<string, string> SelectListItemListProperty { get; set; }
        public bool? NullableBoolProperty { get; set; }
        public DateTime? NullableDateTimeProperty { get; set; }
        public TestEnum? NullableTestEnumProperty { get; set; }
        //[Required]

        /// <summary>
        /// This field also has specific validations in class below
        /// </summary>
        public DateTime? ValidatableDateTimeProperty { get; set; }

        /// <summary>
        /// This field has Validation rules defned (see class below)
        /// </summary>
        public string ValidatableProperty { get; set; }
    }
    public enum TestEnum
    {
        Default = 0,
        Test1 = 103,
        Test2 = 104,
        Test3 = 105,
        Test4 = 306,
        Test5 = 10000
    }

    public enum TestEnumSmall
    {
        Default = 0,
        Test1 = 103,
        Test2 = 104,
        Test3 = 306,
        Test4 = 10000
    }

    /// <summary>
    /// Validation rules, necessary for testing them.
    /// </summary>
    public class DummyViewModelValidator : AbstractValidator<DummyViewModel>
    {
        public DummyViewModelValidator()
        {
            RuleFor(x => x.ValidatableProperty).NotEmpty().Length(0,20);
            RuleFor(x => x.ValidatableDateTimeProperty)
                .NotEmpty()
                .LessThan(DateTime.Now.AddYears(5))
                .GreaterThanOrEqualTo(DateTime.Now.AddYears(-90));
            RuleFor(x => x.DecimalProperty).NotEmpty().WithMessage("Test required message");
        }
    }
}
