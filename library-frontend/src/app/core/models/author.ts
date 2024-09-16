import { Guid } from "guid-typescript";

export interface Author {
    id: Guid;
    name: string;
    surname: string;
    birthday: Date;
    country: string;
}