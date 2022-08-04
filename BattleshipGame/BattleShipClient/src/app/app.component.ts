import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AppService } from './shared/app.service';

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
  grid: any;

  private ctx: CanvasRenderingContext2D;
  boxSize = 60;

  ngOnInit(): void {
    this.ctx = this.canvas.nativeElement.getContext('2d');
    this.drawBox();
  }

  drawBox() {
    this.ctx.beginPath();
    this.ctx.fillStyle = "white";
    this.ctx.lineWidth = 3;
    this.ctx.strokeStyle = "black";
    for (var row = 0; row < 10; row++) {
      for (var column = 0; column < 10; column++) {
        var x = column *  this.boxSize;
        var y = row * this.boxSize;
        this.ctx.rect(x, y, this.boxSize, this.boxSize);
        this.ctx.fill();
        this.ctx.stroke();
      }
    }
    this.ctx.closePath();
  }

  spawnShips() {
    this.appService.loadShips().subscribe((data: any) => {
    });
  }

  getGrid() {
    this.appService.loadGrid().subscribe((data: any) => {
      this.grid = data;
      console.log(this.grid)
    });
  }

  handleClick (e: MouseEvent) {
    console.log(e);
    this.ctx.fillStyle = "black";

    this.ctx.fillRect(Math.floor(e.offsetX / this.boxSize) * this.boxSize,
      Math.floor(e.offsetY / this.boxSize) * this.boxSize,
      this.boxSize, this.boxSize);
  }
}
