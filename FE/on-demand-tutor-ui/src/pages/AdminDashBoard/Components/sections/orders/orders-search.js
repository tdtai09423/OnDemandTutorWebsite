import PropTypes from 'prop-types';
import AdjustmentsHorizontalIcon from '@heroicons/react/24/outline/AdjustmentsHorizontalIcon';
import ListBulletIcon from '@heroicons/react/24/outline/ListBulletIcon';
import Squares2X2Icon from '@heroicons/react/24/outline/Squares2X2Icon';
import {
  Button,
  Stack,
  SvgIcon,
  ToggleButton,
  toggleButtonClasses,
  ToggleButtonGroup
} from '@mui/material';
import { QueryField } from '../../../../../Components/query-field.js';

export const OrdersSearch = (props) => {
  const { mode = 'table', onModeChange, onQueryChange, query } = props;

  return (
    <Stack
      alignItems="center"
      direction="row"
      flexWrap="wrap"
      gap={3}
      sx={{ p: 3 }}
    >
      <QueryField
        placeholder="Search..."
        onChange={onQueryChange}
        sx={{
          flexGrow: 1,
          order: {
            xs: 1,
            sm: 2
          }
        }}
        value={query}
      />

    </Stack>
  );
};

OrdersSearch.propTypes = {
  mode: PropTypes.oneOf(['table', 'dnd']),
  onModeChange: PropTypes.func,
  onQueryChange: PropTypes.func,
  query: PropTypes.string
};
