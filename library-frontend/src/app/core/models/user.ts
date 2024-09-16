import { Guid } from "guid-typescript"
import { Book } from "./book";

export interface UserRole {
    id: Guid;
    name: string;
}

export interface User {
    id: Guid;
    username: string;
    email: string;
    roles: UserRole[];
    books: Book[];
}