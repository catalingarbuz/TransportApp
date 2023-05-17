import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { BookingTable } from "@presentation/components/ui/Tables/BookingsTable/BookingTable";

export const BookingsPage = memo(() => {
  return <Fragment>
    <Seo title="MobyLab Web App | Bookings" />
    <WebsiteLayout>
    <Box sx={{
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          padding: "0px 300px 00px 300px",
          justifyItems: "center",
          height: "100vh",
          width: "100%",
          position: "absolute",
          backgroundImage: "url('src/presentation/assets/img/background2.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center"}}>
        <Box sx={{ padding: "0px 50px 00px 50px", justifyItems: "center"}}>
          <ContentCard>
            <BookingTable />
          </ContentCard>
        </Box>
    </Box>
    </WebsiteLayout>
  </Fragment>
});
