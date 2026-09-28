import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Fragment, memo } from "react";
import { Box } from "@mui/system";
import { Seo } from "@presentation/components/ui/Seo";
import { useIntl } from "react-intl";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import MailIcon from '@mui/icons-material/Mail';
import PhoneIcon from '@mui/icons-material/Phone';
import "./ContactPage.css";


interface ContactProps {
  name: string;
  email: string;
  phone: string;
}

const Contact: React.FC<ContactProps> = ({ name, email, phone }) => {
  return (
    <div className="contact-info">
      <h3>{name}</h3>
         <p><MailIcon /> <a href={`mailto:${email}`}>{email}</a></p>
         <p><PhoneIcon /> <a href={`tel:${phone.replace(/[^\d+]/g, "")}`}>{phone}</a></p>
    </div>
  );
};

const ContactsPage: React.FC = () => {
  const { formatMessage } = useIntl();
  const contacts: ContactProps[] = [
    { name: 'Garbuz Cătălin', email: 'garbuzcatalin.su@gmail.com', phone: '+373 69 422 837' },
    { name: 'Transport Company', email: 'transportcompany@example.com', phone: '987-654-3210' },
  ];

  return (
    <div>
      <Box className="contact-page-background">
        <Box className="contact-page-content">
          <ContentCard>
            <h2 className="contact-card-title">{formatMessage({ id: "globals.contacts" })}</h2>
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
    <Seo title="Transport Company | Contact" />
    <WebsiteLayout>
      <ContactsPage />
    </WebsiteLayout>
  </Fragment>
});
