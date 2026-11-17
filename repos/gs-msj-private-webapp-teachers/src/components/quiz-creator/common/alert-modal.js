import { createPortal } from 'react-dom';
import Alert from 'react-s-alert';
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/slide.css';

export default function AlertModal() {
  return createPortal(
    <Alert stack={{ limit: 3, spacing: 50 }} />,
    document.body
  );
}
