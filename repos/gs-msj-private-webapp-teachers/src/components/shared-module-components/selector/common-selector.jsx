import ReactSelect from 'react-select';

const customStyles = (color) => ({
  control: (base, state) => ({
    ...base,
    minHeight: '35px',
    ...(state.selectProps.isMulti ? {} : { height: '40px' }),
    backgroundColor: color ?? '#EFEFEF',
    marginRight: '10px',
  }),
  container: (base) => ({
    ...base,
    width: '100%',
  }),
    singleValue: (base) => ({
    ...base,
    fontSize: '13px',
  }),
  placeholder: (base) => ({
    ...base,
    fontSize: '13px',
  }),
  option: (base) => ({
    ...base,
    fontSize: '13px',
  }),
  menu: (base) => ({
    ...base,
    fontSize: '13px',
  }),
});

// Custom Select component that applies the custom styles and allows for additional props to be passed in
const Select = ({ styles, color, ...props }) => (
  <ReactSelect
    styles={{
      ...customStyles(color),
      ...styles,
    }}
    {...props}
  />
);

export default Select;