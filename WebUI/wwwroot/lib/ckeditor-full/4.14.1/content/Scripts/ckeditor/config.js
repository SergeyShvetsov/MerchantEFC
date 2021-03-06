/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

	//var roxyFileman = '/fileman/index.html?integration=ckeditor';
	//config.filebrowserBrowseUrl = roxyFileman;
	//config.filebrowserImageBrowseUrl = roxyFileman + '&type=image';
	config.removeDialogTabs = 'link:upload;image:upload';
	config.extraPlugins = "base64image";
};
