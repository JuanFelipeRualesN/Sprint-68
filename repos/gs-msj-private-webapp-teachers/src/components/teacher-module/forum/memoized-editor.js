import React, {forwardRef} from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';

// Allow parent to access the instance with ref
const MemoizedEditor = forwardRef(({ 
  value, 
  onChange, 
  onKeyDown, 
  modules
}, ref) => {
  return (
      <ReactQuill
          theme="snow"
          modules={modules}
          onKeyDown={onKeyDown}
          ref={ref}
          value={value}
          onChange={onChange}
      />
  );
});
// Memoize the component to avoid re-rendering
export const Editor = React.memo(MemoizedEditor);