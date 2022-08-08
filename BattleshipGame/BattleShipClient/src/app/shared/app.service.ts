import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Player } from "./player";
import { Ship } from "./ship";
import { Cell } from "./cell";
import { Board } from "./board";

@Injectable()
export class AppService {

    constructor(public http:HttpClient) {
    }

    spawnBoard() {
        return this.http.get<Board>('http://localhost:5000/api/ship/spawn');
    }

    loadPlayer(id: number) {
        return this.http.get<Player>(`http://localhost:5000/api/game/${id}`);
    }

    shoot(ship: Ship, cell: Cell, cells: Cell[]) {
        return this.http.post<any>('http://localhost:5000/api/ship/shoot', {ship, cell, cells});
    }

    generateAIShot(board: Board, cells: Cell[]) {
        return this.http.post<any>('http://localhost:5000/api/ship/computer', {board, cells});
    }

}