import { ConstantPool } from '@angular/compiler';
import { escapeRegExp } from '@angular/compiler/src/util';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AppService } from './shared/app.service';
import { Board } from './shared/board';
import { Cell } from './shared/cell';
import { Player } from './shared/player';
import { Ship } from './shared/ship';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'BattleShipClient';

  constructor (private appService: AppService) {
  }
  
  @ViewChild('canvasShots', { static: true })
  canvasShots: ElementRef<HTMLCanvasElement>;
  @ViewChild('canvasPlayer', { static: true })
  canvasPlayer: ElementRef<HTMLCanvasElement>;
  private ctxShots: CanvasRenderingContext2D;
  private ctxPlayer: CanvasRenderingContext2D;
  
  player_1: Player;
  player_2: Player;
  p1Ships: number;
  p2Ships: number;
  private gameStarted: boolean = false;
  gameOver: boolean = false;
  private aiShots: Cell[] = [];
  private playerShots: Cell[] = [];
  private imageHit;
  private imageMiss;
  private cellSize = 60;

  ngOnInit(): void {
    this.imageHit = new Image();
    this.imageMiss = new Image();
    this.imageHit.src = "./assets/images/hitMark.svg";
    this.imageMiss.src = "./assets/images/missMark.svg";

    this.ctxShots = this.canvasShots.nativeElement.getContext('2d');
    this.ctxPlayer = this.canvasPlayer.nativeElement.getContext('2d');
    this.drawShotsGrid();
    this.drawGridCoordinates();
    this.drawPlayerGrid();
  }

  clearGrids() {
    this.ctxShots.clearRect(0, 0, 660, 660);
    this.ctxPlayer.clearRect(0, 0, 300, 300);
    this.drawShotsGrid();
    this.drawGridCoordinates();
    this.drawPlayerGrid();
  }

  drawShotsGrid() {
    this.ctxShots.beginPath();
    this.ctxShots.fillStyle = "white";
    this.ctxShots.lineWidth = 3;
    this.ctxShots.strokeStyle = "black";
    for (var row = 0; row < 11; row++) {
      for (var column = 0; column < 11; column++) {
        var x = column *  this.cellSize;
        var y = row * this.cellSize;
        this.ctxShots.rect(x, y, this.cellSize, this.cellSize);
        this.ctxShots.fill();
        this.ctxShots.stroke();
      }
    }
    this.ctxShots.closePath();
  }

  drawGridCoordinates() {
    this.ctxShots.font = "400 25px arial";
    this.ctxShots.fillStyle = "black";

    for (var column = 0; column < 9; column++) {
      var x = 83 + column * this.cellSize;
      this.ctxShots.fillText(`${column+1}`, x, 37);
    }
    this.ctxShots.fillText('10', 615, 37);

    var rowValues = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J']
    for (var row = 0; row < 10; row++) {
      var y = 100 + row * this.cellSize;
      this.ctxShots.fillText(`${rowValues[row]}`, 23, y);
    }
  }

  drawPlayerGrid() {
    this.ctxPlayer.beginPath();
    this.ctxPlayer.fillStyle = "white";
    this.ctxPlayer.lineWidth = 3;
    this.ctxPlayer.strokeStyle = "black";
    for (var row = 0; row < 10; row++) {
      for (var column = 0; column < 10; column++) {
        var x = column *  this.cellSize / 2;
        var y = row * this.cellSize / 2;
        this.ctxPlayer.rect(x, y, this.cellSize / 2, this.cellSize / 2);
        this.ctxPlayer.fill();
        this.ctxPlayer.stroke();
      }
    }
    this.ctxPlayer.closePath();
  }

  drawPlayerShips() {
    this.ctxPlayer.font = "300 25px arial";
    this.ctxPlayer.fillStyle = "black";
    for (var row = 0; row < 10; row++) {
      for (var column = 0; column < 10; column++) {
        if (this.player_1.board.grid[column][row] !== 0) {
          this.ctxPlayer.fillText(`${this.player_1.board.grid[column][row]}`,
           row * this.cellSize / 2 + 7, column * this.cellSize/ 2 + 25)
        }
      }
    }
  }

  checkIfGameOver() {
    if (this.p1Ships === 0) {
      this.gameStarted = false;
      this.gameOver = true;
      alert("COMPUTER WIN!");
    }
    else if (this.p2Ships === 0) {
      this.gameStarted = false;
      this.gameOver = true;
      alert("YOU WIN!");
    }
  }

  AIShot() {
    if (this.gameStarted === true) {
      this.appService.generateAIShot(this.player_1.board, this.aiShots).subscribe((data: any) => {
        this.aiShots = data.cells;
        const ship = data.ship;
        const idx = this.aiShots.length-1
        const xPos = this.aiShots[idx].position[0];
        const yPos = this.aiShots[idx].position[1];

        if (ship.id !== 0) {
          const index = ship.id - 1;
          this.player_1.board.ships[index] = ship;
          console.log("Computer hit your ship:", ship.name);
          this.ctxPlayer.drawImage(this.imageHit, xPos * this.cellSize / 2,
            yPos * this.cellSize / 2, this.cellSize / 2, this.cellSize / 2);
          if (ship.hitpoints === 0) {
            this.p1Ships--;
            console.log("Player 1 ships alive:", this.p1Ships);
            this.checkIfGameOver();
          }
        }
        else {
          console.log("Computer missed");
          this.ctxPlayer.drawImage(this.imageMiss, xPos * this.cellSize / 2,
            yPos * this.cellSize / 2, this.cellSize / 2, this.cellSize / 2);
        }
      });
    }
  }

  shoot (e: MouseEvent) {
    const xPos = Math.floor(e.offsetX / this.cellSize);
    const yPos = Math.floor(e.offsetY / this.cellSize);
    
    if (this.gameStarted === true) {
      if ((yPos !== 0) && (xPos !== 0)) {
        if (this.player_2.board.grid[yPos - 1][xPos - 1] !== 0) {
          const id = this.player_2.board.grid[yPos - 1][xPos - 1];
          const ship = this.player_2.board.ships[id-1];
          this.appService.shoot(ship, { shipId: id, position: [xPos - 1, yPos - 1]}, this.playerShots)
            .subscribe((data: any) => {
              if (data.ship.id !== 0) {
                const index = ship.id - 1;
                this.player_2.board.ships[index] = data.ship;
                this.playerShots = data.cells;
                this.ctxShots.drawImage(this.imageHit, xPos * this.cellSize,
                  yPos * this.cellSize, this.cellSize, this.cellSize)
                if (data.ship.hitpoints === 0) {
                  this.p2Ships--;
                  console.log("Player 2 ships alive:", this.p2Ships);
                  this.checkIfGameOver();
                }
                this.AIShot();
              }
            });
        } 
        else {
          this.appService.shoot({ id: 0, name: "", hitpoints: 0, cells: []}, { shipId: 0, position: [xPos - 1, yPos - 1]}, this.playerShots)
            .subscribe((data: any) => {
              if (data.ship === null) {
                this.playerShots = data.cells;
                this.ctxShots.drawImage(this.imageMiss, xPos * this.cellSize,
                  yPos * this.cellSize, this.cellSize, this.cellSize)
                  this.AIShot();
              }
            })
        }
      }
    }
  }

  getPlayers() {
    this.appService.loadPlayer(1).subscribe((data: Player) => {
      this.player_1 = data;
      console.log(this.player_1)
      this.drawPlayerShips();
    });

    this.appService.loadPlayer(2).subscribe((data: Player) => {
      this.player_2 = data;
      console.log(this.player_2)
    });

    this.clearGrids();
    this.aiShots = [];
    this.playerShots = [];
    this.gameStarted = true;
    this.gameOver = false;
    this.p1Ships = 5;
    this.p2Ships = 5;
  }
}
