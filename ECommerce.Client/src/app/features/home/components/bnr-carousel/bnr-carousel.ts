import { Component, OnInit, ViewChild } from '@angular/core';
import { BannerDto } from '../../../../core/models/banner.model';
import { BannerService } from '../../../home/services/banner-service'
import { NgbCarousel, NgbSlide, NgbSlideEvent, NgbSlideEventSource } from '@ng-bootstrap/ng-bootstrap/carousel';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-bnr-carousel',
  imports: [NgbCarousel, NgbSlide, FormsModule],
  templateUrl: './bnr-carousel.html',
  styleUrl: './bnr-carousel.css',
  providers:[BannerService]
})
export class BnrCarousel implements OnInit {
    public Bnrs: BannerDto[] = [];
   public bnrsImgsUrls :string[]=[]
  	//bnrsImgsUrls = [62, 83, 466, 965, 982, 1043, 738].map((n) => `https://picsum.photos/id/${n}/900/500`);


    constructor(public MyService: BannerService) { }

  ngOnInit(): void {this.getBnrs();}

  getBnrs(): void {
    this.MyService.getBnrs().subscribe({
      next: (data) => {
        console.log(data);
        this.Bnrs = [...data];
       this.bnrsImgsUrls=this.Bnrs.map(e=>e.imageUrl)
        console.log("Banners are:")
        console.log(this.Bnrs)
        console.log("Images are:")
        console.log(this.bnrsImgsUrls)
      },
      error: (err) => {
        console.error(err);
      }
    }); //End of function
  }
  //End of Read//
	paused = false;
	unpauseOnArrow = false;
	pauseOnIndicator = false;
	pauseOnHover = true;
	pauseOnFocus = true;

	@ViewChild('carousel', { static: true }) carousel?: NgbCarousel;

	togglePaused() {
		if (this.paused) {
			this.carousel?.cycle();
		} else {
			this.carousel?.pause();
		}
		this.paused = !this.paused;
	}

	onSlide(slideEvent: NgbSlideEvent) {
		if (
			this.unpauseOnArrow &&
			slideEvent.paused &&
			(slideEvent.source === NgbSlideEventSource.ARROW_LEFT || slideEvent.source === NgbSlideEventSource.ARROW_RIGHT)
		) {
			this.togglePaused();
		}
		if (this.pauseOnIndicator && !slideEvent.paused && slideEvent.source === NgbSlideEventSource.INDICATOR) {
			this.togglePaused();
		}
	}

} //End of class component
