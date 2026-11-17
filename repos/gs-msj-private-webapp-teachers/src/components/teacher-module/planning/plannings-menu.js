import React from 'react';
import { MENU_SECTIONS } from './shared/constants/menu';
import { translate } from './shared/utils';
import MenuItems from './shared/components/menu/menu-items';
import MenuContainer from './shared/components/menu/menu-container';
import MenuSection from './shared/components/menu/menu-section';
import MenuMovilContainer from './shared/components/menu/menu-movil-container';
import PreviewItem from './shared/components/menu/preview-item';

export default function PlanningsMenu ({ children }) {
    const Items = () => {
        return (
            <>
                {MENU_SECTIONS.map((title, index) => {
                    return (
                        <MenuSection key={`section-${index}`} title={title}>
                            <MenuItems section={index}></MenuItems>
                        </MenuSection>
                    );
                })}
            </>
        );
    };

    return (
        <div className="containerPlanning">
            <div className="containerPlanning__sidebar">
                <MenuContainer title={translate('menu.title')}>
                    <PreviewItem />
                    <Items />
                </MenuContainer>

                <MenuMovilContainer title={translate('menu.title')}>
                    <PreviewItem />
                    <Items />
                </MenuMovilContainer>
            </div>
            {children}
        </div>
    );
};
