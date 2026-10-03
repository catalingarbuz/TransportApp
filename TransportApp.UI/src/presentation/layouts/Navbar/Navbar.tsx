import { useCallback } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import HomeIcon from '@mui/icons-material/Home';
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import { Link } from 'react-router-dom';
import { AppRoute } from 'routes';
import { useIntl } from 'react-intl';
import { useAppDispatch, useAppSelector } from '@application/store';
import { useQueryClient } from '@tanstack/react-query';
import { resetProfile } from '@application/state-slices';
import { useAppRouter } from '@infrastructure/hooks/useAppRouter';
import { NavbarLanguageSelector } from '@presentation/components/ui/NavbarLanguageSelector/NavbarLanguageSelector';
import { useOwnUserHasRole } from '@infrastructure/hooks/useOwnUser';
import { UserRoleEnum } from '@infrastructure/apis/client';

/**
 * This is the navigation menu that will stay at the top of the page.
 */
export const Navbar = () => {
  const { formatMessage } = useIntl();
  const { loggedIn } = useAppSelector(x => x.profileReducer);
  const isAdmin = useOwnUserHasRole(UserRoleEnum.Admin);
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  const { redirectToHome } = useAppRouter();
  const logout = useCallback(() => {
    dispatch(resetProfile());
    redirectToHome();
  }, [queryClient, dispatch, redirectToHome]);

  return <Box>
    <AppBar>
      <Toolbar sx={{ gap: 2, flexWrap: 'wrap' }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
          <Link to={AppRoute.Index} aria-label="Home">
            <HomeIcon sx={{ color: 'white', display: 'block' }} fontSize="large" />
          </Link>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
            {isAdmin && <>
              <Button color="inherit" component={Link} to={AppRoute.Users}>
                {formatMessage({ id: "globals.users" })}
              </Button>
              <Button color="inherit" component={Link} to={AppRoute.UserFiles}>
                {formatMessage({ id: "globals.files" })}
              </Button>
              <Button color="inherit" component={Link} to={AppRoute.Routes}>
                {formatMessage({ id: "globals.routes" })}
              </Button>
              <Button color="inherit" component={Link} to={AppRoute.Cars}>
                {formatMessage({ id: "globals.cars" })}
              </Button>
              <Button color="inherit" component={Link} to={AppRoute.Drivers}>
                {formatMessage({ id: "globals.drivers" })}
              </Button>
            </>}
            {loggedIn && <>
              <Button color="inherit" component={Link} to={AppRoute.Bookings}>
                {formatMessage({ id: "globals.bookings" })}
              </Button>
              <Button color="inherit" component={Link} to={AppRoute.Contact}>
                {formatMessage({ id: "globals.contacts" })}
              </Button>
            </>}
          </Box>
        </Box>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, marginLeft: 'auto'}}>
          <NavbarLanguageSelector />
          {!loggedIn && <>
            <Button color="inherit" component={Link} to={AppRoute.Login}>
              {formatMessage({ id: "globals.login" })}
            </Button>
            <Button color="inherit" component={Link} to={AppRoute.Signup}>
              {formatMessage({ id: "globals.signup" })}
            </Button>
          </>}
          {loggedIn && <Button onClick={logout} color="inherit">
            <LogoutOutlinedIcon sx={{ color: 'white', display: 'block' }} fontSize="large" />
          </Button>}
        </Box>
      </Toolbar>
    </AppBar>
  </Box>
}