import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgxParticlesModule } from '@tsparticles/angular';
import { Engine } from '@tsparticles/engine';
import { loadSlim } from '@tsparticles/slim';

@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet, NgxParticlesModule],
  templateUrl: './auth-layout.html',
  styleUrl: './auth-layout.css',
})
export class AuthLayout {

  particlesId = 'auth-particles';

particlesOptions = {
  fpsLimit: 60,
  particles: {
    number: { value: 150, density: { enable: true, value_area: 800 } },
    color: { value: '#75A5B7' },
    links: {
      enable: true,
      distance: 150,
      color: '#75A5B7',
      opacity: 1,
      width: 1
    },
    move: { enable: true, speed: 2 },
    size: { value: 3, random: true },
    opacity: { value: 0.5 }
  },
  interactivity: {
    events: {
      onHover: { enable: true, mode: 'grab' },
      onClick: { enable: true, mode: 'push' }
    },
    modes: {
      grab: { distance: 140, links: { opacity: 1 } },
      push: { quantity: 4 }
    }
  },
  retina_detect: true
};
  async particlesInit(engine: Engine) {
    await loadSlim(engine);
  }
}
