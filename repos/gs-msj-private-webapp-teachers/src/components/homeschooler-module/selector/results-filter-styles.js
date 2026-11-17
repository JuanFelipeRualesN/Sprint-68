import { makeStyles } from "@material-ui/core";

// Styles for the MUI components used in the lesson filters component
export const styles = makeStyles(theme => ({
    truncatedLabel: {
        maxWidth: '100px',
        whiteSpace: 'nowrap',
        overflow: 'hidden',
        fontSize: '13px',
        textOverflow: 'ellipsis',
        display: 'block',
        [theme.breakpoints.down(751)]: {
            maxWidth: '100%',
            whiteSpace: 'normal',
            fontSize: '11.5px',
        },
    },
    truncatedLabelTablet: {
        maxWidth: '100%',
        whiteSpace: 'normal',
        fontSize: '11.5px',
    },
    checkboxContainer: {
        display: 'flex',
        width: '100%',
        flexWrap: 'wrap',
    },
    titles: {
        fontWeight: 600,
        fontSize: '15px',
    },
    searchContainer: {
        padding: '15px',
        marginTop: '5px'
    },
    searchButton: {
        fontSize: '13px',
        fontWeight: 600,
    },
    checkboxColumn: {
        display: 'flex',
        flexDirection: 'column',
    },
    accordionDetails:{
        padding: '10px 16px 0 16px',
    },
    accordionSummary: {
        borderBottom: '2px solid #E8E8E8',
        padding: '0',
        margin: '0 16px 0 16px',
    },
    accordion: {
        margin: 0,
        '&:before': {
            display: 'none',
        },
        backgroundColor: '#fafafa',
        width: '100%',
        minHeight: '50px',
        [theme.breakpoints.down('xs')]: {
            minWidth: '100%',
        },
    },
    inputRow: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'start',
        marginBottom: '20px',
    },
    scrollableContainer: {
        maxHeight: '200px',
        overflowY: 'auto',
        overflowX: 'hidden',
        width: '100%',
        '&::-webkit-scrollbar': {
            width: '4px',
        },
        '&::-webkit-scrollbar-track': {
            background: '#f1f1f1',
        },
        '&::-webkit-scrollbar-thumb': {
            background: '#888',
            borderRadius: '4px',
        },
        '&::-webkit-scrollbar-thumb:hover': {
            background: '#555',
        }
    },
    customCheckbox: {
        '&.Mui-checked': {
            color: '#456083'
        },
        '&:hover': {
            backgroundColor: 'rgba(69, 96, 131, 0.08)',
        },
        '&.Mui-checked:hover': {
            backgroundColor: 'rgba(69, 96, 131, 0.12)',
        },
    },
}));