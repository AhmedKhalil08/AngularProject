import {
  DestroyRef,
  assertInInjectionContext,
  inject
} from "./chunk-NUEU6FYJ.js";
import {
  Observable,
  takeUntil
} from "./chunk-Z47DVUQS.js";

// ../../../../node_modules/@angular/core/fesm2022/rxjs-interop.mjs
function takeUntilDestroyed(destroyRef) {
  if (!destroyRef) {
    ngDevMode && assertInInjectionContext(takeUntilDestroyed);
    destroyRef = inject(DestroyRef);
  }
  const destroyed$ = new Observable((subscriber) => {
    if (destroyRef.destroyed) {
      subscriber.next();
      return;
    }
    const unregisterFn = destroyRef.onDestroy(subscriber.next.bind(subscriber));
    return unregisterFn;
  });
  return (source) => {
    return source.pipe(takeUntil(destroyed$));
  };
}

export {
  takeUntilDestroyed
};
//# sourceMappingURL=chunk-3WG24LBX.js.map
