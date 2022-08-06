import { Cell } from "./cell";

export class Ship {
    id: number;
    name: string;
    size: number;
    hitpoints: number;
    cells: Cell[];
}
