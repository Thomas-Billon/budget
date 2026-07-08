interface IResetPasswordRequest {
    email: string;
    token: string;
    newPassword: string;
}

export { type IResetPasswordRequest };
