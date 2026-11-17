import React from 'react';
import { useTranslation } from 'react-i18next';
import ToolModal from "../tools/tool-modal/tool-modal.jsx";
import { TOOLS } from "./tool-config/tools-config";
import Ic from "../../../../../assets/svg/icons";

/* AI Tools Panel — tabs: Planning · Differentiation · Assessment · Reading · Assistants */

// AI_DATA is derived from tools-config.TOOLS at module load time, so adding a
// new tool there automatically surfaces it here (and in AIToolsPage). No more
// drift between the home grid and the dedicated AI Tools page.

// Category keys — English slugs only. Labels are resolved via i18next at render time.
const CATEGORY_KEYS = [
    'planning',
    'differentiation',
    'assessment',
    'reading',
    'assistants',
];

// Soft tint per category — matches the AIToolsPage palette for consistency.
const CAT_TINT = {
    planning:        { bg: '#E9F2FA', fg: '#27466C' },
    differentiation: { bg: '#FFE9D6', fg: '#A8521A' },
    assessment:      { bg: '#EEE9FF', fg: '#6745EA' },
    reading:         { bg: '#DFF5E8', fg: '#246B49' },
    assistants:      { bg: '#FFF0D6', fg: '#B16C00' },
};

// Per-tool icon mapping keyed by the stable English tool identifiers in tools-config.
const ICON_BY_KEY = {
    'plan-my-lesson':           Ic.doc,
    'stem-unit-planner':        Ic.flask,
    'adapt-lesson':             Ic.layers,
    'language-objectives':      Ic.target,
    'group-strategy':           Ic.groups,
    'differentiate-content':    Ic.layers,
    'differentiate-process':    Ic.sliders,
    'differentiate-product':    Ic.trophy,
    'intervention-plan':        Ic.heart,
    'create-rubric':            Ic.pending,
    'dok-questions':            Ic.brain,
    'math-exam':                Ic.calc,
    'personalized-feedback':    Ic.msg2,
    'choice-board':             Ic.board,
    'assessment-creator':       Ic.check,
    'diagnostic-tests':         Ic.target,
    'leveled-reading':          Ic.book,
    'academic-vocabulary':      Ic.abc,
    'comprehension-questions':  Ic.help,
    'lexile-assessment':        Ic.ruler,
    'math-assistant':           Ic.calc,
    'science-assistant':        Ic.flask,
    'ela-assistant':            Ic.book,
    'social-studies-assistant': Ic.globe,
    'language-assistant':       Ic.speech,
    'special-ed-coach':         Ic.heart,
    'ell-coach':                Ic.globe,
};

// Build the AI_DATA map from TOOLS config. Each category bucket holds its
// items with tint colors and icons resolved once at module load time.
// Category labels are NOT resolved here — they are translated at render time.
const AI_DATA = (() => {
    const out = {};
    for (const [key, cfg] of Object.entries(TOOLS)) {
        const cat = cfg.category || 'planning';
        if (!out[cat]) out[cat] = { items: [] };
        const tint = CAT_TINT[cat] || CAT_TINT.planning;
        out[cat].items.push({
            key,
            Icon: ICON_BY_KEY[key] || Ic.sparkle,
            bg: tint.bg,
            fg: tint.fg,
        });
    }
    // Preserve a sensible category order on the tabs even if TOOLS iteration
    // order doesn't already match (object iteration is insertion order in modern
    // JS, but be explicit here).
    return Object.fromEntries(
        CATEGORY_KEYS.filter(k => out[k]).map(k => [k, out[k]])
    );
})();

function AITools() {
    const { t } = useTranslation(['ai-tools', 'common']);

    const tabs = Object.keys(AI_DATA);
    const [active, setActive] = React.useState('planning');
    const [openTool, setOpenTool] = React.useState(null);

    const data = AI_DATA[active];
    const isAssistants = active === 'assistants';

    // Open the tool modal if the tool is configured; otherwise show a notification.
    const openIfConfigured = (key) => {
        const cfg = TOOLS?.[key];
        if (cfg) {
            setOpenTool(cfg);
        } else {
            // TODO: replace alert() with the project's shared notification/toast component
            alert(t('ai-tools:card.notConfigured', { name: t(`ai-tools:tools.${key}.name`) }));
        }
    };

    return (
        <section className="ai-panel">
            <div className="ai-head">
                <div className="lhs">
                    <h2>
                        <span className="sparkle"><Ic.sparkle /></span>
                        {t('ai-tools:panel.title')}
                    </h2>
                    <p>{t('ai-tools:panel.subtitle')}</p>
                </div>

                {/* Category tab bar */}
                <div className="ai-tabs" role="tablist">
                    {tabs.map((tabKey) => (
                        <button
                            key={tabKey}
                            role="tab"
                            className={`ai-tab ${active === tabKey ? 'active' : ''}`}
                            onClick={() => setActive(tabKey)}
                        >
                            {t(`ai-tools:categories.${tabKey}`)}
                            <span className="count">{AI_DATA[tabKey].items.length}</span>
                        </button>
                    ))}
                </div>
            </div>

            {/* Tool cards grid — assistant cards use a different layout */}
            <div className={`ai-grid ${isAssistants ? 'assistants' : ''}`}>
                {data.items.map((item, i) => {
                    const configured = !!TOOLS?.[item.key];

                    return isAssistants ? (
                        <article
                            key={item.key}
                            className={`ai-card assistant ${configured ? '' : 'soon'}`}
                            style={{ animation: `fadeUp .5s ${i * 40}ms both` }}
                            onClick={() => openIfConfigured(item.key)}
                        >
                            <div className="av"><item.Icon /></div>
                            <div className="ai-name">{t(`ai-tools:tools.${item.key}.name`)}</div>
                            <div className="ai-desc">{t(`ai-tools:tools.${item.key}.subtitle`)}</div>
                            <button
                                className="chat-now"
                                onClick={(e) => { e.stopPropagation(); openIfConfigured(item.key); }}
                            >
                                {configured ? t('ai-tools:card.chat') : t('common:general.comingSoon')} <Ic.arrow />
                            </button>
                        </article>
                    ) : (
                        <article
                            key={item.key}
                            className={`ai-card ${configured ? '' : 'soon'}`}
                            style={{ animation: `fadeUp .5s ${i * 40}ms both` }}
                            onClick={() => openIfConfigured(item.key)}
                        >
                            <div className="ai-icon"><item.Icon /></div>
                            <div className="ai-name">{t(`ai-tools:tools.${item.key}.name`)}</div>
                            <div className="ai-desc">{t(`ai-tools:tools.${item.key}.subtitle`)}</div>
                            <button
                                className="ai-use"
                                onClick={(e) => { e.stopPropagation(); openIfConfigured(item.key); }}
                            >
                                {configured ? t('ai-tools:card.use') : t('common:general.comingSoon')} <Ic.arrow />
                            </button>
                        </article>
                    );
                })}
            </div>

            {openTool && <ToolModal tool={openTool} onClose={() => setOpenTool(null)} />}
        </section>
    );
}

export default AITools;
