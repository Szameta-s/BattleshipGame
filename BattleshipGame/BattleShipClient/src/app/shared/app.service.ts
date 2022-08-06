import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Player } from "./player";
import { Ship } from "./ship";
import { Cell } from "./cell";

@Injectable()
export class AppService {

    constructor(public http:HttpClient) {
    }

    loadShips() {
        return this.http.get<any>('http://localhost:5000/api/ship/spawn');
    }

    loadPlayer() {
        return this.http.get<Player>('http://localhost:5000/api/game');
    }

    shoot(cell: Cell) {
        return this.http.post<Ship>('http://localhost:5000/api/ship/shoot', cell);
    }

}