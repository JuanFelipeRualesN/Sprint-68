import { useState } from 'react';

// Lets an accordion load its data when it opens instead of on mount.
const useAccordionLoad = (accordionId) => {
    const [open, setOpen] = useState('');
    const [opened, setOpened] = useState(false);
    const [openCount, setOpenCount] = useState(0);

    const toggle = (id) => {
        const next = open === id ? '' : id;
        setOpen(next);

        if (next === accordionId) {
            setOpened(true);
            setOpenCount((current) => current + 1);
        }
    };

    return { open, toggle, opened, openCount };
};

export default useAccordionLoad;
