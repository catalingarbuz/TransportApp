import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Typography } from "@mui/material";
import { Fragment, memo } from "react";
import { useIntl } from "react-intl";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { Button } from "@mui/material"

export const HomePage = memo(() => {
  const { formatMessage } = useIntl();

  return <Fragment>
      <Seo title="MobyLab Web App | Home" />
      <Box sx={{
        height: "100vh",
        backgroundImage: "url('src/presentation/assets/img/background.jpg')",
        backgroundSize: "cover",
        backgroundPosition: "center"
      }}>
        <WebsiteLayout>
          <Box sx={{ padding: "150px 50px 0px 50px", justifyItems: "center" }}>
            <ContentCard title={formatMessage({ id: "globals.welcome" })}>
              <Typography style={{ fontSize: '20px', color: 'black' }} >
                { formatMessage({id: "globals.appdescription"}) }
              </Typography>
            </ContentCard>
          </Box>
          <Box sx={{ padding: "25px 0px 0px 0px", display: "flex", justifyContent: "center" }}>
              <Button variant="contained" sx={{ height: "40px", width: "200px", fontSize: "16px" }}>{formatMessage({ id: "globals.bookASeat" })}</Button>
          </Box>
        </WebsiteLayout>
      </Box>
    </Fragment>
});