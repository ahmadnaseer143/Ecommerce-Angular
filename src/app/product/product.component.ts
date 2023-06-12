import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent {
  @Input() view: 'grid' | 'list' | 'currcartitem' | 'prevcartitem' = 'grid';

}
