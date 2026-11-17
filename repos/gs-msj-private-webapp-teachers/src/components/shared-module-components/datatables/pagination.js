import React, { useState, useEffect } from 'react';
import Pagination from '@material-ui/lab/Pagination';

const Paginationn = ({ currentPage, totalPages, updateParent, dapper }) => {

    if (!dapper) {
        return (<PaginationnNotDapper totalPages={totalPages} currentPage={currentPage} updateParent={updateParent} />);
    }
    return (<PaginationnDapper totalPages={totalPages} currentPage={currentPage} updateParent={updateParent} />);
};

export default Paginationn;

const PaginationnDapper = ({ currentPage, totalPages, updateParent }) => {
    const [page, setPage] = useState(currentPage + 1);

    useEffect(() => {
        setPage(currentPage + 1);
    }, [currentPage]);

    const handleChange = (event, value) => {
        updateParent(value - 1);
        setPage(value);
    };

    return (
        <Pagination count={totalPages} page={page} onChange={handleChange} />
    );
};

const PaginationnNotDapper = ({ currentPage, totalPages, updateParent }) => {

    const handleChange = (event, value) => {
        updateParent(value);
    };

    return (
        <Pagination count={totalPages} page={currentPage} onChange={handleChange} />
    );
};
