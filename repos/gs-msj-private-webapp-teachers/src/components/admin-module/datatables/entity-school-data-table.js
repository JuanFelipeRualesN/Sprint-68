/**
 * Created by portiz on 1/31/20.
 */
import DataTable from 'react-data-table-component';
import React from 'react';
import {withLocalize} from "react-localize-redux";

const EntityDataTable = ({ columns, data, title, updateParent }) => {
    const handleOnRowClick = (e) => {
        updateParent(e);
    };

    return (
        <DataTable
            onRowClicked={handleOnRowClick}
            columns={columns}
            data={data}
            pagination={true}
            paginationPerPage={5}
            striped={true}
            paginationRowsPerPageOptions={[5,10,15,20,30,60,100]}
        />
    );
};

export default withLocalize(EntityDataTable);