import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.css']
})
export class OffersComponent {
  offerForm !: FormGroup;
  showFormValue: Boolean = false;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.offerForm = this.formBuilder.group({
      id: [1, Validators.required],
      title: ['', Validators.required],
      discount: [0, Validators.required],
    })

    this.loadOffers();
  }

  loadOffers() {
    // get all the offers
  }

  addOffer() {
    // add offer
  }

  showForm() {
    this.showFormValue = !this.showFormValue;
  }

}
