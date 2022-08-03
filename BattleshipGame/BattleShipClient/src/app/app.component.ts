import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'BattleShipClient';
  
  @ViewChild('canvas', { static: true })
  canvas: ElementRef<HTMLCanvasElement>;  
  
  private ctx: CanvasRenderingContext2D;
  

  ngOnInit(): void {
    this.ctx = this.canvas.nativeElement.getContext('2d');
    // this.ctx.fillStyle = "#66ccff";
    // this.ctx.fillRect(0, 0, 600, 600);
  }
  
  animate(): void {}
}
