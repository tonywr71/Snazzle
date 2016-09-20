import { Component, Input, OnInit } from '@angular/core';

import { Speaker } from '../../../models';

@Component({
  selector: 'ev-dashboard-button',
  template: require('dashboard-button.component.html'),
  styles: [String(require('dashboard-button.component.css'))]
})
export class DashboardButtonComponent implements OnInit {
  @Input() speaker: Speaker;

  constructor() {}

  ngOnInit() {
  }
}
