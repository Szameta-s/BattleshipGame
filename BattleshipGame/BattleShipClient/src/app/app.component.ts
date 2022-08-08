import { ConstantPool } from '@angular/compiler';
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
  
  @ViewChild('canvas', { static: true })
  canvas: ElementRef<HTMLCanvasElement>;
  private ctx: CanvasRenderingContext2D;
  
  player_1: Player;
  player_2: Player;
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

    this.ctx = this.canvas.nativeElement.getContext('2d');
    this.drawGrid();
    this.drawCoordinates();
  }

  drawCoordinates() {
    this.ctx.font = "400 25px arial";
    this.ctx.fillStyle = "black";

    for (var column = 0; column < 9; column++) {
      var x = 83 + column * this.cellSize;
      this.ctx.fillText(`${column+1}`, x, 37);
    }
    this.ctx.fillText('10', 615, 37);

    var rowValues = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J']
    for (var row = 0; row < 10; row++) {
      var y = 100 + row * this.cellSize;
      this.ctx.fillText(`${rowValues[row]}`, 23, y);
    }
  }

  drawGrid() {
    this.ctx.beginPath();
    this.ctx.fillStyle = "white";
    this.ctx.lineWidth = 3;
    this.ctx.strokeStyle = "black";
    for (var row = 0; row < 11; row++) {
      for (var column = 0; column < 11; column++) {
        var x = column *  this.cellSize;
        var y = row * this.cellSize;
        this.ctx.rect(x, y, this.cellSize, this.cellSize);
        this.ctx.fill();
        this.ctx.stroke();
      }
    }
    this.ctx.closePath();
  }

  AIShot() {
    this.appService.generateAIShot(this.player_1.board, this.aiShots).subscribe((data: any) => {
      this.aiShots = data.cells;
      const ship = data.ship;

      if (ship.id !== 0) {
        const index = ship.id - 1;
        this.player_1.board.ships[index] = ship;
        console.log("Computer hit your ship:", ship.name);
      }
      else {
        console.log("Computer missed");
      }
    });
  }

  drawHitMark (e: MouseEvent) {
    const xPos = Math.floor(e.offsetX / this.cellSize);
    const yPos = Math.floor(e.offsetY / this.cellSize);
    const moveAvailable: boolean = true;


    if ((yPos !== 0) && (xPos !== 0) && moveAvailable) {
      if (this.player_2.board.grid[yPos - 1][xPos - 1] !== 0) {
        const id = this.player_2.board.grid[yPos - 1][xPos - 1];
        const ship = this.player_2.board.ships[id-1];
        this.appService.shoot(
          ship,
          { shipId: id, position: [xPos - 1, yPos - 1]}  
          ).subscribe((data: Ship) => {
              const index = data.id - 1;
              this.player_2.board.ships[index] = data;
              this.ctx.drawImage(this.imageHit, xPos * this.cellSize,
                yPos * this.cellSize, this.cellSize, this.cellSize)
          });
      } 
      else {
        this.ctx.drawImage(this.imageMiss, xPos * this.cellSize,
          yPos * this.cellSize, this.cellSize, this.cellSize)
      }
      this.AIShot();
    }
  }

  getPlayers() {
    this.appService.loadPlayer(1).subscribe((data: Player) => {
      this.player_1 = data;
      console.log(this.player_1.board.ships)
    });

    this.appService.loadPlayer(2).subscribe((data: Player) => {
      this.player_2 = data;
      console.log(this.player_2.board.ships)
    });
  }
}
