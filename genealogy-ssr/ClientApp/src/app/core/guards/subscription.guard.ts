import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngxs/store';
import { MainState } from '../../states/main.state';

@Injectable({ providedIn: 'root' })
export class SubscriptionGuard  {
  constructor(private router: Router, private store: Store) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const hasSubscribe = this.store.selectSnapshot(MainState.hasSubscription);
    if (hasSubscribe) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
    return hasSubscribe;
  }
}
