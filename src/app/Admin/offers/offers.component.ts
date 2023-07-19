import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Offer } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-offers',
  templateUrl: './offers.component.html',
  styleUrls: ['./offers.component.css']
})
export class OffersComponent {
  offerForm !: FormGroup;
  showFormValue: Boolean = false;
  offerList: Offer[] = [];
  itemsPerPage: number = 8;
  p: number = 1;

  constructor(private formBuilder: FormBuilder, private navigationService: NavigationService) { }

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
    this.navigationService.getAllOffers().subscribe((res: any) => {
      this.offerList = res;
    },
      error => console.log("Error in getting all offers in add product", error))

  }

  addOffer() {
    this.offerForm.markAllAsTouched();
    if (this.offerForm.invalid) {
      alert("Validation Error");
      return;
    }
    // add offer
    this.navigationService.insertOffer(this.offerForm.value).subscribe((res: any) => {
      this.offerForm.reset();
      this.showFormValue = false;
      this.loadOffers();
    }, error => console.log("Error in adding offer", error))
  }

  showForm() {
    this.showFormValue = !this.showFormValue;
  }

}
