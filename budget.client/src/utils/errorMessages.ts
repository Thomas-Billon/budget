export const emailMaxLength = 256;
export const nameMaxLength = 64;
export const passwordMinLength = 8;
export const passwordMaxLength = 128;

export const passwordSpecialChars = [
    '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',',
    '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\',
    ']', '^', '_', '`', '{', '|', '}', '~'
];

export const errorMessages: Record<string, string> =
{
    'error.field.required': 'This field is required.',
    'error.email.too_long': `Email must be at most ${emailMaxLength} characters.`,
    'error.email.invalid_format': 'Please enter a valid email address.',
    'error.name.too_long': `Name must be at most ${nameMaxLength} characters.`,
    'error.password.too_short': `Password must be at least ${passwordMinLength} characters.`,
    'error.password.too_long': `Password must be at most ${passwordMaxLength} characters.`,
    'error.password.missing_uppercase': 'Password must contain at least one uppercase letter.',
    'error.password.missing_lowercase': 'Password must contain at least one lowercase letter.',
    'error.password.missing_digit': 'Password must contain at least one digit.',
    'error.password.missing_special': `Password must contain at least one special character (${passwordSpecialChars.join(' ')}).`,
    'error.auth.invalid_credentials': 'Invalid credentials.',
    'error.auth.registration_failed': 'Registration failed. Please check your details and try again.',
    'error.auth.token_expired': 'Your session has expired. Please log in again.',
    'error.auth.reset_password_failed': 'This reset link is invalid or has expired. Please request a new one.',
    'error.auth.email_confirmation_failed': 'This confirmation link is invalid or has expired. Please request a new one.',
    'error.transaction.not_found': 'Transaction not found.',
    'error.transaction.cannot_create': 'Failed to create the transaction.',
    'error.transaction.cannot_update': 'Failed to update the transaction.',
    'error.transaction.cannot_patch': 'Failed to update the transaction.',
    'error.transaction.cannot_delete': 'Failed to delete the transaction.',
    'error.category.not_found': 'Category not found.',
    'error.category.cannot_create': 'Failed to create the category.',
    'error.category.cannot_update': 'Failed to update the category.',
    'error.category.cannot_patch': 'Failed to update the category.',
    'error.category.cannot_delete': 'Failed to delete the category.',
    'error.server.internal': 'An unexpected error occurred. Please try again later.',
    'error.server.not_implemented': 'This feature is not yet available.',
    'error.server.unauthorized': 'You are not authorized to perform this action.',
    'error.server.too_many_requests': 'Too many attempts. Please wait a moment and try again.'
};
