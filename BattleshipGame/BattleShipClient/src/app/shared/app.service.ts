import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable()
export class AppService {

    constructor(public http:HttpClient) {
    }

    loadShips() {
        return this.http.get<any>('http://localhost:5000/api/ship/pos');
    }

    loadGrid() {
        return this.http.get<any>('http://localhost:5000/api/game');
    }

}