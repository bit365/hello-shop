namespace HelloShop.ServiceDefaults.Infrastructure;

public class CustomFluentValidationLanguageManager : FluentValidation.Resources.LanguageManager
{

    public CustomFluentValidationLanguageManager()
    {
        AddTranslation("en-US", "EmailValidator", "{PropertyName} is invalid");
        AddTranslation("en-US", "GreaterThanOrEqualValidator", "{PropertyName} must be greater than or equal to {ComparisonValue}");
        AddTranslation("en-US", "GreaterThanValidator", "{PropertyName} must be greater than {ComparisonValue}");
        AddTranslation("en-US", "LengthValidator", "{PropertyName} must be between {MinLength} and {MaxLength} characters");
        AddTranslation("en-US", "MinimumLengthValidator", "The length of {PropertyName} must be at least {MinLength} characters");
        AddTranslation("en-US", "MaximumLengthValidator", "The length of {PropertyName} must be {MaxLength} characters or fewer");
        AddTranslation("en-US", "LessThanOrEqualValidator", "{PropertyName} must be less than or equal to {ComparisonValue}");
        AddTranslation("en-US", "LessThanValidator", "{PropertyName} must be less than {ComparisonValue}");
        AddTranslation("en-US", "NotEmptyValidator", "{PropertyName} must not be empty");
        AddTranslation("en-US", "NotEqualValidator", "{PropertyName} must not be equal to {ComparisonValue}");
        AddTranslation("en-US", "NotNullValidator", "{PropertyName} must not be empty");
        AddTranslation("en-US", "PredicateValidator", "The specified condition was not met for {PropertyName}");
        AddTranslation("en-US", "AsyncPredicateValidator", "The specified condition was not met for {PropertyName}");
        AddTranslation("en-US", "RegularExpressionValidator", "{PropertyName} is not in the correct format");
        AddTranslation("en-US", "EqualValidator", "{PropertyName} must be equal to {ComparisonValue}");
        AddTranslation("en-US", "ExactLengthValidator", "{PropertyName} must be {MaxLength} characters in length");
        AddTranslation("en-US", "InclusiveBetweenValidator", "{PropertyName} must be between {From} and {To}");
        AddTranslation("en-US", "ExclusiveBetweenValidator", "{PropertyName} must be between {From} and {To}");
        AddTranslation("en-US", "CreditCardValidator", "{PropertyName} is not a valid credit card number");
        AddTranslation("en-US", "ScalePrecisionValidator", "{PropertyName} must not be more than {ExpectedPrecision} digits in total with allowance for {ExpectedScale} decimals");
        AddTranslation("en-US", "EmptyValidator", "{PropertyName} must be empty");
        AddTranslation("en-US", "NullValidator", "{PropertyName} must be empty");
        AddTranslation("en-US", "EnumValidator", "{PropertyName} has a range of values which does not include {PropertyValue}");
        AddTranslation("en-US", "Length_Simple", "{PropertyName} must be between {MinLength} and {MaxLength} characters");
        AddTranslation("en-US", "MinimumLength_Simple", "The length of {PropertyName} must be at least {MinLength} characters");
        AddTranslation("en-US", "MaximumLength_Simple", "The length of {PropertyName} must be {MaxLength} characters or fewer");
        AddTranslation("en-US", "ExactLength_Simple", "{PropertyName} must be {MaxLength} characters in length");
        AddTranslation("en-US", "InclusiveBetween_Simple", "{PropertyName} must be between {From} and {To}");

        AddTranslation("zh-CN", "EmailValidator", "{PropertyName}必须是有效的邮件格式");
        AddTranslation("zh-CN", "GreaterThanOrEqualValidator", "{PropertyName}必须大于等于{ComparisonValue}");
        AddTranslation("zh-CN", "GreaterThanValidator", "{PropertyName}必须大于{ComparisonValue}");
        AddTranslation("zh-CN", "LengthValidator", "{PropertyName}必须{MinLength}-{MaxLength}个字符");
        AddTranslation("zh-CN", "MinimumLengthValidator", "{PropertyName}至少{MinLength}个字符");
        AddTranslation("zh-CN", "MaximumLengthValidator", "{PropertyName}最多{MaxLength}个字符");
        AddTranslation("zh-CN", "LessThanOrEqualValidator", "{PropertyName}必须小于等于{ComparisonValue}");
        AddTranslation("zh-CN", "LessThanValidator", "{PropertyName}必须小于{ComparisonValue}");
        AddTranslation("zh-CN", "NotEmptyValidator", "{PropertyName}不能为空");
        AddTranslation("zh-CN", "NotEqualValidator", "{PropertyName}不能和{ComparisonValue}相等");
        AddTranslation("zh-CN", "NotNullValidator", "{PropertyName}不能为空");
        AddTranslation("zh-CN", "PredicateValidator", "{PropertyName}不符合指定的条件");
        AddTranslation("zh-CN", "AsyncPredicateValidator", "{PropertyName}不符合指定的条件");
        AddTranslation("zh-CN", "RegularExpressionValidator", "{PropertyName}格式不正确");
        AddTranslation("zh-CN", "EqualValidator", "{PropertyName}应该和{ComparisonValue}相等");
        AddTranslation("zh-CN", "ExactLengthValidator", "{PropertyName}必须是{MaxLength}个字符");
        AddTranslation("zh-CN", "InclusiveBetweenValidator", "{PropertyName}必须在{From}-{To}之间");
        AddTranslation("zh-CN", "ExclusiveBetweenValidator", "{PropertyName}必须在{From}-{To}之间");
        AddTranslation("zh-CN", "CreditCardValidator", "{PropertyName}不是有效的信用卡号");
        AddTranslation("zh-CN", "ScalePrecisionValidator", "{PropertyName}不能超过{ExpectedPrecision}位，小数部分{ExpectedScale}位");
        AddTranslation("zh-CN", "EmptyValidator", "{PropertyName}必须为空");
        AddTranslation("zh-CN", "NullValidator", "{PropertyName}必须为空");
        AddTranslation("zh-CN", "EnumValidator", "{PropertyName}范围不包含{PropertyValue}");
        AddTranslation("zh-CN", "Length_Simple", "{PropertyName}必须是{MinLength}-{MaxLength}个字符");
        AddTranslation("zh-CN", "MinimumLength_Simple", "{PropertyName}至少{MinLength}个字符");
        AddTranslation("zh-CN", "MaximumLength_Simple", "{PropertyName}最多{MaxLength}个字符");
        AddTranslation("zh-CN", "ExactLength_Simple", "{PropertyName}必须是{MaxLength}个字符");
        AddTranslation("zh-CN", "InclusiveBetween_Simple", "{PropertyName}必须在{From}-{To}之间");
    }
}
