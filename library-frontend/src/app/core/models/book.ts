import { Guid } from "guid-typescript";
import { Author } from "./author";

export interface Book {
    id: Guid;
    name: string;
    description: string;
    genre: string;
    isbn: string;
    returnBy?: Date;
    takenAt?: Date;
    imagePath?: string;

    authors: Author[];
}