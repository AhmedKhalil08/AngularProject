import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgxParticlesModule } from '@tsparticles/angular';
import { Engine } from '@tsparticles/engine';
import { loadBubblesPreset } from '@tsparticles/preset-bubbles';

@Component({
  selector: 'app-home',
  imports: [NgxParticlesModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  particlesId = 'hero-particles';
    particlesOptions = {
    preset: 'bubbles',
    background: { color: { value: 'transparent' } },
    particles: {
      number: { value: 15 },
      size: { value: { min: 5, max: 20 } },
      opacity: { value: { min: 0.1, max: 0.3 } },
      move: { enable: true, speed: 1 }
    }
  }
  async particlesInit(engine: Engine) {
    await loadBubblesPreset(engine);
  }

  ngOnInit(): void {
    // this.loadBanners();
  }

  //   loadBanners() {
  //   this.api.get<BannerDto[]>('banners').subscribe({
  //     next: (data) => {
  //       this.banners.set(data.filter(b => b.isActive).sort((a, b) => a.displayOrder - b.displayOrder));
  //       this.startAutoSlide();
  //     }
  //   });
  // }
  //   startAutoSlide() {
  //   this.autoSlideInterval = setInterval(() => {
  //     this.nextBanner();
  //   }, 4000);
  // }
  //   nextBanner() {
  //   const total = this.banners().length;
  //   if (total === 0) return;
  //   this.currentBannerIndex.set((this.currentBannerIndex() + 1) % total);
  // }

  // prevBanner() {
  //   const total = this.banners().length;
  //   if (total === 0) return;
  //   this.currentBannerIndex.set((this.currentBannerIndex() - 1 + total) % total);
  // }

  // goToBanner(index: number) {
  //   this.currentBannerIndex.set(index);
  // }

  // ngOnDestroy() {
  //   clearInterval(this.autoSlideInterval);
  // }
}
