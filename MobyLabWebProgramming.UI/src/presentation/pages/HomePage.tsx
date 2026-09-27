import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Typography } from "@mui/material";
import { Fragment, memo } from "react";
import { useIntl } from "react-intl";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { Button } from "@mui/material";
import { useAppSelector } from '@application/store';
import { Link } from 'react-router-dom';
import { AppRoute } from 'routes';

export const HomePage = memo(() => {
  const { formatMessage } = useIntl();
  const { loggedIn } = useAppSelector(x => x.profileReducer);

  return <Fragment>
      <Seo title="Transport Company | Home" />
      
        <WebsiteLayout>
          <Box>
            <Box sx={{ padding: "150px 250px 20px 250px", justifyItems: "center" }}>
              <ContentCard title={formatMessage({ id: "globals.welcome" })}>
                <Typography style={{ fontSize: '20px', color: 'black', textAlign: 'center' }} >
                  { formatMessage({id: "globals.appdescription"}) }
                </Typography>
              </ContentCard>
            </Box>
            <Box sx={{ padding: "25px 0px 0px 0px", display: "flex", justifyContent: "center" }}>
              {!loggedIn && <Button variant="contained" sx={{ height: "40px", width: "200px", fontSize: "16px" }}>
                <Link style={{ color: 'white' }} to={AppRoute.Login}>
                  {formatMessage({ id: "globals.bookASeat" })}
                </Link>
              </Button>}
              {loggedIn && <Button variant="contained" sx={{ height: "40px", width: "200px", fontSize: "16px" }}>
                <Link style={{ color: 'white' }} to={AppRoute.Bookings}>
                  {formatMessage({ id: "globals.bookASeat" })}
                </Link>
              </Button>}
            </Box>
          </Box>
        </WebsiteLayout>

    </Fragment>
});