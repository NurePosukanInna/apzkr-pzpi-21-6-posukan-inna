import React from 'react';
import { useTranslation } from 'react-i18next';
import './languageSwitcher.css';

const LanguageSwitcher = () => {
  const { i18n } = useTranslation();

  const changeLanguage = (lang) => {
    i18n.changeLanguage(lang);
  };

  return (
    <div className="language-switcher">
      <button
        className={i18n.language === 'en' ? 'active' : ''}
        onClick={() => changeLanguage('en')}
      >
        EN
      </button>
      <button
        className={i18n.language === 'ua' ? 'active' : ''}
        onClick={() => changeLanguage('ua')}
      >
        UA
      </button>
    </div>
  );
};

export default LanguageSwitcher;
