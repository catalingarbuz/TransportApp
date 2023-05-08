import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/material";
import { Seo } from "@presentation/components/ui/Seo";
import { SignupForm } from "@presentation/components/forms/SignUp/SignupForm";

export const SignupPage = memo(() => {
    return <Fragment>
        <Seo title="MyTransport | Sign-up" />
        <WebsiteLayout>
            <Box sx={{ padding: "0px 50px 0px 50px", justifyItems: "center" }}>
                <SignupForm />
            </Box>
        </WebsiteLayout>
    </Fragment>
});
