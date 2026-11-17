import { v4 as uuid } from 'uuid';

/**
 * A publish-subscribe system for managing topics and subscribers.
 */
export const Subject = (() => {
  const state = {
    subscribers: Object.create(null),
    activeSubscribers: Object.create(null),
  };

  /**
   * Sets the active subscriber for a specific topic.
   * @param {{ topic: string, subscriberId: string }} params
   */
  function setActiveSubscriber({ topic, subscriberId }) {
    if (!state.subscribers[topic])
      throw new Error(`Topic "${topic}" does not exist.`);

    state.activeSubscribers[topic] = subscriberId;
  }

  /**
   * Subscribes a new callback function to a specific topic.
   * @param {{ topic: string, callback: Function, subscriberId?: string }} params
   * @returns {string} The subscriber identifier.
   */
  function subscribe({ topic, callback, subscriberId }) {
    if (!state.subscribers[topic]) {
      state.subscribers[topic] = {};
    }

    if (state.subscribers[topic][subscriberId])
      throw new Error(`Subscriber with identifier '${subscriberId}' already exists for topic '${topic}'.`);

    subscriberId ??= uuid();

    state.subscribers[topic][subscriberId] = callback;
    state.activeSubscribers[topic] = subscriberId;

    return subscriberId;
  }

  /**
   * Unsubscribes a callback function from a specific topic.
   * @param {{ topic: string, subscriberId: string }} params
   */
  function unsubscribe({ topic, subscriberId }) {
    if (!state.subscribers[topic] || !state.subscribers[topic][subscriberId])
      return;

    delete state.subscribers[topic][subscriberId];

    if (Object.keys(state.subscribers[topic]).length === 0)
      delete state.subscribers[topic];

  }

  /**
   * Publishes data to all or specific subscribers of a topic.
   * @param {{ topic: string, data: any, pushToActive?: boolean }} params
   */
  function publish({ topic, data, pushToActive }) {
    if (!state.subscribers[topic])
      return;

    if (!pushToActive) {
      const callbacks = Object.values(state.subscribers[topic]);
      callbacks.forEach(callback => callback(data));
      return;
    }

    const activeSubscriberId = state.activeSubscribers[topic];
    if (!activeSubscriberId)
      return;

    const callback = state.subscribers[topic][activeSubscriberId];
    if (callback)
      callback(data);
  }

  const api = { setActiveSubscriber, subscribe, unsubscribe, publish };

  // Keep parity with previous static fields: allow read/write (assignment) on maps
  Object.defineProperties(api, {
    subscribers: {
      get() { return state.subscribers; },
      set(v) { state.subscribers = v || Object.create(null); },
      enumerable: true,
    },
    activeSubscribers: {
      get() { return state.activeSubscribers; },
      set(v) { state.activeSubscribers = v || Object.create(null); },
      enumerable: true,
    },
  });

  return api;
})();
