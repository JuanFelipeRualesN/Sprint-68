import React from "react";
import { useHistory } from "react-router-dom";
import Ic from "../../../../assets/svg/icons";
/* Quick access grid — clean version (no pulse chips, single accent column).
 * Generalized: receives the cards to render (already filtered by role),
 * so the grid stays purely presentational. All labels arrive via props,
 * already resolved through t() by the parent. */

// Brand accent for the enter CTA — matches the orange generate button
// from the right-column AI prompt, keeping the dashboard visually cohesive.
const ENTER_ACCENT = "#E88B19";

/**
 * @param {string}   title       Section heading, e.g. t('dashboard.quick-access')
 * @param {string}   enterLabel  CTA label, e.g. t('dashboard.enter')
 * @param {Array}    cards       [{ id, Icon | iconSrc, alt, bg, fg, title, desc, meta, badge, route }]
 * @param {Function} onNavigate  Optional override; defaults to react-router navigation
 */
function QuickGrid({ title, enterLabel, cards, onNavigate }) {
  const history = useHistory();
  const go = (route) => (onNavigate ? onNavigate(route) : history.push(route));

  return (
    <div>
      <div className="section-head" style={{ marginBottom: 14 }}>
        <div className="lhs">
          <h2>{title}</h2>
        </div>
      </div>

      <div className="qa-grid">
        {cards.map((card, index) => (
          <article
            key={card.id}
            className="qa-card"
            style={{ animationDelay: `${index * 50}ms`, "--card-accent": card.fg }}
          >
            <div className="qa-icon" style={{ background: card.bg, color: card.fg }}>
              {card.Icon ? (
                <card.Icon />
              ) : (
                <img src={card.iconSrc} alt={card.alt} className="qa-icon-img" />
              )}
              {card.badge ? <span className="messages-unread-alert">{card.badge}</span> : null}
            </div>
            <h3>{card.title}</h3>
            <p>{card.desc}</p>
            <div className="qa-foot">
              <div className="meta">{card.meta || ""}</div>
              <button
                className="enter"
                style={{ color: ENTER_ACCENT }}
                onClick={() => go(card.route)}
              >
                {enterLabel} <Ic.arrow />
              </button>
            </div>
          </article>
        ))}
      </div>
    </div>
  );
}

export default QuickGrid;
