const prevPath = (path) => {
  return {
    type: "PREV_PATH",
    payload: path,
  };
};

export default prevPath;
