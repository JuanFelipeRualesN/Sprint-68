import { useState } from "react";
// import { useContext, useState } from "react";
import * as forums from "../services/forums.service";
// import { AppContext } from '../../../App.context';
// import { translate } from '../../shared/utils';

const useCreateForum = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);
  // const { dispatch } = useContext(AppContext);

  const createForum = async (forum) => {
    setIsLoading(true);

    try {
      const response = await forums.createForum(forum);

      setSucceeded(response);
      setIsLoading(false);
      return Promise.resolve(response);
    } catch (error) {
      setSucceeded(false);
      // dispatch({ type: 'OPEN_SNACKBAR', payload: { severity: 'error', message: translate('errors.badRequest') } });
    }

    setIsLoading(false);
    return Promise.resolve(false);
  };

  return {
    createForum,
    isLoading,
    succeeded,
  };
};

export default useCreateForum;
