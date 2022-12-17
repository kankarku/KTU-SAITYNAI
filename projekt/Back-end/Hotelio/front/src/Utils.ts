import { AccessTokenPayload, Role } from "./AuthenticationTypes";
import jwt_decode from "jwt-decode";

export interface JWTAuthToken {
    exp: string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
    sub: string;
  }

function unixTime(): number {
    return Math.floor(Date.now() / 1000);
}

function parseJwt(token: string): AccessTokenPayload {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    const obj = JSON.parse(jsonPayload);
    var trueRole = jwt_decode<JWTAuthToken>(token)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] as Role;
    const accessTokenPayload: AccessTokenPayload = {
        Role: trueRole,
        Id: obj.nameid,
        ExpiresAt: obj.exp,
        Issuer: obj.iss,
        Audience: obj.aud,
        RefreshTokenExpirationTime: parseInt(obj.RefreshTokenExpirationTime),
    };

    return accessTokenPayload;
}

function capText(text: string, maxSize: number): string {
    if (text.length <= maxSize) return text;

    return text.substr(0, maxSize) + ' ...';
}


export { unixTime, parseJwt, capText};