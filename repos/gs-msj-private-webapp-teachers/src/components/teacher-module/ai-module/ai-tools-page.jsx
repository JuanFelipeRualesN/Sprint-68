import React, {useState, useMemo} from 'react';
import {useTranslation} from 'react-i18next';
import {TOOLS} from './components/tools/tool-config/tools-config';
import ToolModal from '../ai-module/components/tools/tool-modal/tool-modal'
import Ic from '../../../assets/svg/icons'
import '../../../styles/design-tokens.css';
import '../../../styles/ai-module-and-dashboard.css';

// Category id slugs — English identifiers only; labels resolved via t() at render time
const CATEGORY_IDS = [
    'all',
    'planning',
    'differentiation',
    'assessment',
    'reading',
    'assistants',
];

// Map category slug → soft background tint for the icon tile
const CAT_TINT = {
    planning: {bg: '#E9F2FA', fg: '#27466C'},
    differentiation: {bg: '#FFE9D6', fg: '#A8521A'},
    assessment: {bg: '#EEE9FF', fg: '#6745EA'},
    reading: {bg: '#DFF5E8', fg: '#246B49'},
    assistants: {bg: '#FFF0D6', fg: '#B16C00'},
};

// Map tool name (English slug from toolsConfig) → icon component
function pickIcon(toolKey) {
    const map = {
        'plan-my-lesson': Ic.doc,
        'stem-unit-planner': Ic.flask,
        'adapt-lesson': Ic.layers,
        'language-objectives': Ic.target,
        'group-strategy': Ic.grupos,
        'differentiate-content': Ic.layers,
        'differentiate-process': Ic.sliders,
        'differentiate-product': Ic.trophy,
        'intervention-plan': Ic.heart,
        'create-rubric': Ic.pendientes,
        'dok-questions': Ic.brain,
        'math-exam': Ic.calc,
        'personalized-feedback': Ic.msg2,
        'choice-board': Ic.board,
        'assessment-creator': Ic.check,
        'diagnostic-tests': Ic.target,
        'leveled-reading': Ic.book,
        'academic-vocabulary': Ic.abc,
        'comprehension-questions': Ic.help,
        'lexile-assessment': Ic.ruler,
        'math-assistant': Ic.calc,
        'science-assistant': Ic.flask,
        'ela-assistant': Ic.book,
        'social-studies': Ic.globe,
        'language-assistant': Ic.speech,
        'special-ed-coach': Ic.heart,
        'ell-coach': Ic.globe,
    };
    return map[toolKey] || Ic.sparkle;
}

export default function AiToolsPage() {
    const {t} = useTranslation(['ai-tools', 'common']);

    const [activeCategory, setActiveCategory] = useState('all');
    const [searchQuery, setSearchQuery] = useState('');
    // openTool: null | { tool: cfg, initialValues?: object }
    const [openTool, setOpenTool] = useState(null);

    function openWith(toolCfg, initialValues = null) {
        setOpenTool({tool: toolCfg, initialValues});
    }

    function handleSwitchTool(toolKey, initialValues = {}) {

        const next = TOOLS[toolKey];
        if (!next) return;
        // Force a fresh modal mount so initialValues take effect
        setOpenTool(null);
        setTimeout(() => setOpenTool({tool: next, initialValues}), 0);
    }

    const visibleTools = useMemo(() => {
        return Object.entries(TOOLS).filter(([key, cfg]) => {
            if (activeCategory !== 'all' && cfg.category !== activeCategory) return false;
            if (searchQuery.trim()) {
                const query = searchQuery.toLowerCase();
                const nameLabel = t(`ai-tools:tools.${key}.name`, {defaultValue: key}).toLowerCase();
                const subtitleLabel = t(`ai-tools:tools.${key}.subtitle`, {defaultValue: ''}).toLowerCase();
                if (!nameLabel.includes(query) && !subtitleLabel.includes(query)) return false;
            }
            return true;
        });
    }, [activeCategory, searchQuery, t]);

    const categoryCounts = useMemo(() => {
        const counts = {all: Object.keys(TOOLS).length};
        for (const cfg of Object.values(TOOLS)) {
            counts[cfg.category] = (counts[cfg.category] || 0) + 1;
        }
        return counts;
    }, []);

    if (openTool) {
        return (
            <div>
                <div className="container-padding animated delay-05s fadeIn">
                    <div className="wrapper-principal-container">

                        <div className="ai-page ai-page-workspace">
                            <ToolModal
                                tool={openTool.tool}
                                initialValues={openTool.initialValues}
                                onClose={() => setOpenTool(null)}
                                onSwitchTool={handleSwitchTool}
                                embedded
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    const totalTools = Object.keys(TOOLS).length;

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="ai-page">
                        <header className="ai-page-head">
                            <div className="lhs">
                                <div className="eyebrow">
                                    <Ic.sparkle/> {t('ai-tools:header.eyebrow')}
                                </div>
                                <h1 className="title-principal">{t('ai-tools:header.title')}</h1>
                                <p>{t('ai-tools:header.description', {count: totalTools})}</p>
                            </div>
                            <div className="search-box">
                                <Ic.search/>
                                <input
                                    value={searchQuery}
                                    onChange={(e) => setSearchQuery(e.target.value)}
                                    placeholder={t('ai-tools:header.searchPlaceholder')}
                                />
                            </div>
                        </header>

                        <nav className="ai-cat-tabs" role="tablist">
                            {CATEGORY_IDS.map((categoryId) => (
                                <button
                                    key={categoryId}
                                    className={`ai-cat-tab ${activeCategory === categoryId ? 'active' : ''}`}
                                    onClick={() => setActiveCategory(categoryId)}
                                    role="tab"
                                >
                                    {t(`ai-tools:categories.${categoryId}`)}
                                    <span className="cat-count">{categoryCounts[categoryId] || 0}</span>
                                </button>
                            ))}
                        </nav>

                        <div className="ai-page-grid">
                            {visibleTools.map(([key, cfg], index) => {
                                const tint = CAT_TINT[cfg.category] || CAT_TINT.planning;
                                const Icon = pickIcon(key);
                                return (
                                    <article
                                        key={key}
                                        className="ai-page-card"
                                        style={{animation: `fadeUp .4s ${index * 25}ms both`}}
                                        onClick={() => openWith(cfg)}
                                    >
                                        <div
                                            className="ai-page-card-icon"
                                            style={{background: tint.bg, color: tint.fg}}
                                        >
                                            <Icon/>
                                        </div>
                                        <div className="ai-page-card-body">
                                            <h3>{t(`ai-tools:tools.${key}.name`, {defaultValue: key})}</h3>
                                            <p>{t(`ai-tools:tools.${key}.subtitle`, {defaultValue: ''})}</p>
                                        </div>
                                        <div className="ai-page-card-foot">
                <span className="ai-page-cat">
                  {t(`ai-tools:categories.${cfg.category}`)}
                </span>
                                            <button
                                                className="ai-page-use"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    openWith(cfg);
                                                }}
                                            >
                                                {cfg.isChat
                                                    ? t('ai-tools:card.chat')
                                                    : t('ai-tools:card.use')
                                                }
                                                {' '}<Ic.arrow/>
                                            </button>
                                        </div>
                                    </article>
                                );
                            })}

                            {visibleTools.length === 0 && (
                                <div className="ai-page-empty">
                                    {t('ai-tools:grid.noResults')}
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
