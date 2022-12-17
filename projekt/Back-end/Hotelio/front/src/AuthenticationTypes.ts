
enum Role {
    Owner = "Owner",
    Admin = "Admin",
    User = "User",
}

type LoginRequest = {
    Email: string,
    Password: string,
}

type RegisterRequest = {
    Email: string,
    Password: string,
    Username: string,
}

type RefreshTokenRequest = {
    AccessToken: string,
    RefreshToken: string,
}

type AuthenticatedResponse = {
    accessToken: string,
    refreshToken: string,
}

interface User {
    Id: number,
    Role: Role,
    Email: string,
    AccessToken: string,
    RefreshToken: string,
    AccessTokenExpiresAt: number,
    RefreshTokenExpiresAt: number,
}

type AccessTokenPayload = {
    Role: Role,
    Id: number,
    ExpiresAt: number,
    Issuer: string,
    Audience: string,
    RefreshTokenExpirationTime: number,
}

enum ContextStatus {
    Idle,
    Pending,
    Completed,
    Rejected,
}

export type { LoginRequest, RegisterRequest, RefreshTokenRequest, AuthenticatedResponse, User, AccessTokenPayload }
export { ContextStatus, Role }