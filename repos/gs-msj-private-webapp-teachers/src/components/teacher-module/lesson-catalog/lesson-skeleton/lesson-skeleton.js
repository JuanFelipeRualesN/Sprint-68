import React from 'react';
import { Card, CardContent } from '@material-ui/core';
import Skeleton from '@material-ui/lab/Skeleton';

// Skeleton component for lesson cards when data is loading
const LessonSkeleton = ({ count = 8 }) => {
  const skeletons = Array.from({ length: count }, () => (
    <div style={{ margin: '5px', width: '310px' }}>
      <Card>
        <Skeleton variant="rect" height={180} animation="wave" />
        <CardContent>
          <Skeleton variant="text" height={30} width="90%" animation="wave" />
          <Skeleton variant="text" height={15} width="60%" animation="wave" />      
          <Skeleton variant="text" height={15} width="60%" animation="wave" />
        </CardContent>
      </Card>
    </div>
  ));

  return (
    <div className="lessons-container">
      {skeletons}
    </div>
  );
};

export default LessonSkeleton;