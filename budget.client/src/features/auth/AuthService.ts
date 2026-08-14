const EMAIL_FORMAT_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const EMAIL_MAX_LENGTH = 256;

const NAME_MAX_LENGTH = 64;

const PASSWORD_MIN_LENGTH = 8;
const PASSWORD_MAX_LENGTH = 128;
const PASSWORD_SPECIAL_CHARS = [
    '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',',
    '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\',
    ']', '^', '_', '`', '{', '|', '}', '~'
];

const validateEmail = (value: string): string[] => {
    if (!value) {
        return ['error.field.required'];
    }

    const errors: string[] = [];

    if (value.length > EMAIL_MAX_LENGTH) {
        errors.push('error.email.too_long');
    }
    if (!EMAIL_FORMAT_REGEX.test(value)) {
        errors.push('error.email.invalid_format');
    }

    return errors;
};

const validatePasswordLength = (value: string): string[] => {
    if (!value) {
        return ['error.field.required'];
    }

    const errors: string[] = [];

    if (value.length < PASSWORD_MIN_LENGTH) {
        errors.push('error.password.too_short');
    }
    if (value.length > PASSWORD_MAX_LENGTH) {
        errors.push('error.password.too_long');
    }

    return errors;
};

const validatePassword = (value: string): string[] => {
    const lengthErrors = validatePasswordLength(value);
    if (lengthErrors.length > 0) {
        return lengthErrors;
    }

    const errors: string[] = [];

    if (!/[A-Z]/.test(value)) {
        errors.push('error.password.missing_uppercase');
    }
    if (!/[a-z]/.test(value)) {
        errors.push('error.password.missing_lowercase');
    }
    if (!/[0-9]/.test(value)) {
        errors.push('error.password.missing_digit');
    }
    if (!PASSWORD_SPECIAL_CHARS.some(char => value.includes(char))) {
        errors.push('error.password.missing_special');
    }

    return errors;
};

const validateOptionalName = (value: string): string[] => {
    if (value.length > NAME_MAX_LENGTH) {
        return ['error.name.too_long'];
    }

    return [];
};

export {
    EMAIL_FORMAT_REGEX,
    EMAIL_MAX_LENGTH,
    NAME_MAX_LENGTH,
    PASSWORD_MIN_LENGTH,
    PASSWORD_MAX_LENGTH,
    PASSWORD_SPECIAL_CHARS,
    validateEmail,
    validatePassword,
    validatePasswordLength,
    validateOptionalName
};
