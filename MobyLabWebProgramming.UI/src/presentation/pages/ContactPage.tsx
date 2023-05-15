import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { useIntl } from "react-intl";
import { ContentCard } from "@presentation/components/ui/ContentCard";


interface ContactProps {
  name: string;
  email: string;
  phone: string;
}

const Contact: React.FC<ContactProps> = ({ name, email, phone }) => {
  return (
    <div>
      <h3>{name}</h3>
      <p>Email: {email}</p>
      <p>Phone: {phone}</p>
    </div>
  );
};

const ContactsPage: React.FC = () => {
  const { formatMessage } = useIntl();
  const contacts: ContactProps[] = [
    { name: 'Garbuz Catalin', email: 'garbuzcatalin.su@gmail.com', phone: '123-456-7890' },
    { name: 'MyTransApp', email: 'transportapp@example.com', phone: '987-654-3210' },
  ];

  return (
    <div>
      <Box sx={{
          position: "fixed",
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          padding: "0px 300px 00px 300px",
          justifyItems: "center",
          height: "100vh",
          width: "100%",
          backgroundImage: "url('src/presentation/assets/img/background4.jpg')",
          backgroundSize: "cover",
          backgroundPosition: "center"}}>
      <Box sx={{ padding: "100px 160px 00px 320px", justifyItems: "center" }}>
      <ContentCard>
      <h2>{formatMessage({ id: "globals.contacts" })}</h2>
      {contacts.map((contact, index) => (
        <Contact key={index} {...contact} />
      ))}
      </ContentCard>
      </Box>
      </Box>
    </div>
  );
};

export default ContactsPage;




export const ContactPage = memo(() => {
  return <Fragment>
    <Seo title="MobyLab Web App | Contact" />
    <WebsiteLayout>
      <ContactsPage />
    </WebsiteLayout>
  </Fragment>
});
