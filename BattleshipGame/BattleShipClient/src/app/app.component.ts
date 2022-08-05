import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AppService } from './shared/app.service';
import { Grid } from './shared/grid';
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
  
  showShips: boolean = false;
  grid: Grid;
  ships: Ship[];
  private imageHit;
  private imageMiss;
  private xPos: number;
  private yPos: number;
  cellSize = 60;

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

  drawHitMark (e: MouseEvent) {
    this.xPos = Math.floor(e.offsetX / this.cellSize);
    this.yPos = Math.floor(e.offsetY / this.cellSize);
    
    if ((this.yPos !== 0) && (this.xPos !== 0)) {
      if (this.grid.Board[this.yPos - 1][this.xPos - 1] === 1) {
        this.ctx.drawImage(this.imageHit, this.xPos * this.cellSize,
          this.yPos * this.cellSize, this.cellSize, this.cellSize)
      } 
      else {
        this.ctx.drawImage(this.imageMiss, this.xPos * this.cellSize,
          this.yPos * this.cellSize, this.cellSize, this.cellSize)
      }
    }
  }

  spawnShips() {
    this.appService.loadShips().subscribe((data: any) => {
    });
  }

  getGrid() {
    this.appService.loadGrid().subscribe((data: Grid) => {
      this.grid = data;
      this.ships = data.Ships;
      this.showShips = true;
      console.log(this.grid)
    });
  }
}
